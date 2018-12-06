using Helpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helpa.Services.Interface
{
    public interface IMyReviewServices
    {
        Task<NetworkModel> GetReviewToReview(int UserId);
        Task<MyReviewModel> GetReviewAboutMe(int UserId);
        Task<MyReviewModel> GetReviewByMe(int UserId);
    }
}
