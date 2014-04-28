using System;

namespace SaasOvation.Collaboration.Application.Forums.Data {
    public class PostData {
        public string AuthorEmailAddress { get; set; }
        public string Authoridentity { get; set; }
        public string AuthorName { get; set; }
        public string BodyText { get; set; }
        public DateTime ChangedOn { get; set; }
        public DateTime CteatedOn { get; set; }
        public string DiscussionId { get; set; }
        public string ForumId { get; set; }
        public string PostId { get; set; }
        public string ReplyToPostId { get; set; }
        public string Subject { get; set; }
        public string TenantId { get; set; }
    }
}