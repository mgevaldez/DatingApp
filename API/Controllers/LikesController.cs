using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(ILikesRepository likesRepo) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself.");

            var existingLike = await likesRepo.GetUserLikeAsync(sourceUserId, targetUserId);
            if (existingLike == null)
            {
                var like = new UserLike {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };
                likesRepo.AddLike(like);
            } else 
            {
                likesRepo.DeleteLike(existingLike);
            }

            if (await likesRepo.SaveChanges()) return Ok();
            return BadRequest("Failed to update like.");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await likesRepo.GetCurrentUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParam likesParam)
        {
            likesParam.UserId = User.GetUserId();
            var users = await likesRepo.GetUserLikes(likesParam);
            
            Response.AddPagination(users);
            return Ok(users);
        }
    }
}