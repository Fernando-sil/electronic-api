using System.Security.Claims;
using api.DataContext;
using api.DTO.RatingDTO;
using api.Entities;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class RatingRepository : IRatingRepository
{
    private readonly DatabaseContext _context;
    private readonly ICurrentUserInterface _currentUser;

    public RatingRepository(DatabaseContext context, ICurrentUserInterface currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    public async Task<ResponseRepository<GetRatingDTO>> AddRating(AddRatingDTO addRatingDTO)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<GetRatingDTO>();
        var currentUser = await _currentUser.GetUser();
        if(currentUser is null){
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }
        var currentItem = await _context.Items.Include(rating => rating.Ratings).FirstOrDefaultAsync(item => item.Id == addRatingDTO.ItemId);
        if(currentItem is null){
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
        }
        var rating = addRatingDTO.ToRating();
        rating.ItemId = currentItem.Id;
        rating.UserId = currentUser.Id;
        _context.Ratings.Add(rating);
        var itemRatings = currentItem.Ratings;
        var itemRatingCount = currentItem.Ratings!.Count == 0 ? 1 : currentItem.Ratings.Count;
        var itemRating = itemRatings!.Sum(rating=>rating.Rate);
        currentItem.Score = itemRating/itemRatingCount;
        await _context.SaveChangesAsync();
        response.Data = (GetRatingDTO)rating;
        return response;
    }
    public async Task<ResponseRepository<string>> DeleteRating(int id)
    {
        var failResponse = new FailResponse();
        var response = new ResponseRepository<string>();
        var userId = await _currentUser.GetUserId();
        if (userId is null)
        {
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }
        var rating = await _context.Ratings.FirstOrDefaultAsync(rating => rating.Id == id && rating.UserId == userId);
        if (rating is null)
        {
            failResponse.GenerateFailResponse(response, "Rating not found or user not authorized");
            return response;
        }
        var item = await _context.Items.Include(rating => rating.Ratings).FirstOrDefaultAsync(p => p.Id == rating.ItemId);
        if (item is null)
        {
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
        }
        SetNewItemScore(rating, item);
        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();
        response.Data = "Rating deleted";
        return response;
    }

    public async Task<ResponseRepository<UpdateRatingDTO>> UpdateRating(UpdateRatingDTO updateRatingDTO, int id)
    {
        var response = new ResponseRepository<UpdateRatingDTO>();
        var failResponse = new FailResponse();
        var user = await _currentUser.GetUser();
        if(user is null){
            failResponse.GenerateFailResponse(response, "User not found");
            return response;
        }
        var dbRating = await _context.Ratings.
        FirstOrDefaultAsync(rate => rate.Id == id && rate.UserId == user.Id);
        if(dbRating is null){
            failResponse.GenerateFailResponse(response, "Rating not found");
            return response;
        }
        var item = await _context.Items.Include(rating => rating.Ratings).FirstOrDefaultAsync(p => p.Id == dbRating.ItemId);
         if(item is null){
            failResponse.GenerateFailResponse(response, "Item not found");
            return response;
        }
        SetNewItemScore(dbRating, item, updateRatingDTO);
        await _context.SaveChangesAsync();
        var rating = new UpdateRatingDTO{
           Rate = dbRating.Rate,
           Comment = dbRating.Comment
        };
        response.Data = rating;
        return response;
    }

    public async Task<ResponseRepository<GetRatingDTO>> GetRating(int id)
    {
        var response = new ResponseRepository<GetRatingDTO>();
        var failResponse = new FailResponse();
        var dbRating = await _context.Ratings.Include(user => user.User).Include(item => item.Item).FirstOrDefaultAsync(spec => spec.Id == id);
         if(dbRating is null){
            failResponse.GenerateFailResponse(response, "Rating not found");
            return response;
        }
        var rating = (GetRatingDTO)dbRating;
        response.Data = rating;
        return response;
    }
    private static void SetNewItemScore(Rating rating, Item item)
    {
        var oldRate = item.Ratings!.Find(rate => rate.Id == rating.Id)!.Rate;
        var ratingCount = item.Ratings.Count;
        var ratingSum = item.Ratings.Sum(rating => rating.Rate);
        var denominator = ratingCount == 1 ? 2 : ratingCount;
        item.Score = (ratingSum - oldRate) / (denominator-1);
    }
    private static void SetNewItemScore(Rating rating, Item item, UpdateRatingDTO updateRatingDTO)
    {
        var oldRate = item.Ratings!.Find(rate => rate.Id == rating.Id)!.Rate;
        var ratingCount = item.Ratings.Count;
        var ratingSum = item.Ratings.Sum(rating => rating.Rate);
        item.Score = (ratingSum - oldRate + updateRatingDTO.Rate)/ratingCount;    
    }
}