using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Helpa.Models
{
    public class MyReviewModel
    {
        public ObservableCollection<ReviewModel> Data { get; set; }
        public int Total { get; set; }
    }

    public class ReviewModel
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ReviewCommentId { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime Datetime { get; set; }
        public List<ReviewModel> ReviewData { get; set; }
    }
}
