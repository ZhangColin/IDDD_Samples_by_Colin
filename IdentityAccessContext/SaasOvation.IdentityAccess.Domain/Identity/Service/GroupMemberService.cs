using System.Linq;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Domain.Identity.Service {
    public interface IGroupMemberService {
        bool ConfirmUser(Group group, User user);
        bool IsMemberGroup(Group group, GroupMember groupMember);
        bool IsUserInNestedGroup(Group group, User user);
    }

    public class GroupMemberService: IGroupMemberService {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupMemberService(IUserRepository userRepository, IGroupRepository groupRepository) {
            this._userRepository = userRepository;
            this._groupRepository = groupRepository;
        }

        public bool ConfirmUser(Group group, User user) {
            User confirmedUser = _userRepository.UserWithUserName(group.TenantId, user.UserName);
            return confirmedUser != null && confirmedUser.IsEnabled;
        }

        public bool IsMemberGroup(Group group, GroupMember groupMember) {
            bool isMember = false;

            foreach(GroupMember member in group.GroupMembers.Where(m=>m.IsGroup)) {
                if(groupMember.Equals(member)) {
                    isMember = true;
                }
                else {
                    Group nestedGroup = _groupRepository.GroupNamed(member.TenantId, member.Name);
                    if(nestedGroup!=null) {
                        isMember = this.IsMemberGroup(nestedGroup, groupMember);
                    }
                }

                if(isMember) {
                    break;
                }
            }

            return isMember;
        }

        public bool IsUserInNestedGroup(Group group, User user) {
            foreach(GroupMember member in group.GroupMembers.Where(m=>m.IsGroup)) {
                Group nestedGroup = _groupRepository.GroupNamed(member.TenantId, member.Name);
                if(nestedGroup!=null) {
                    bool isInNestedGroup = nestedGroup.IsMember(user, this);
                    if(isInNestedGroup) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}