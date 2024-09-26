using api.Repository;

namespace api.Helpers;

public class FailResponse
{
    public void GenerateFailResponse<T>(ResponseRepository<T> response, string message){
        response.Success = false;
        response.Message = message;
    }
    public void GenerateFailResponse<T>(ResponseRepositoryWithCount<T> response, string message){
        response.Count = 0;
        response.Success = false;
        response.Message = message;
    }
}