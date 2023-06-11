using Business.Abstract;
using Core.CommonModel;
using Core.CommonModel.Result;
using Entities.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS_API.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTS_API.Controllers
{
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private IUserService _userService;
    public UserController(IUserService userService)
        {
            _userService = userService;
        }

    [HttpPost]
    [Route("AddWorker")]
    public ServiceResult AddWorker(RegisterViewModel vm)  //Roller nerede???
    {
      ServiceResult res = new ServiceResult();

      var tokenRes = TokenHelper.TokenCoz(vm.AccessToken);

      if (tokenRes.StatusCode == ResultCode.Unauthorized)
      {
        res.StatusCode = tokenRes.StatusCode;
        res.Message = tokenRes.Message;
        return res;
      }
      res = _userService.Add(vm, 1);
      return res;
    }

    [HttpPost]
    [Route("AddWorker2")]
    public ServiceResult AddWorker2(RegisterViewModel vm)
    {
      ServiceResult res = new ServiceResult();

      var tokenRes = TokenHelper.TokenCoz(vm.AccessToken);

      if (tokenRes.StatusCode == ResultCode.Unauthorized)
      {
        res.StatusCode = tokenRes.StatusCode;
        res.Message = tokenRes.Message;
        return res;
      }
      res = _userService.Add(vm, 1);
      return res;
    }


    [HttpPut]
    [Route("UpdateWorker")]
    public ServiceResult<UserViewModel> UpdateWorker(UserViewModel vm, string accessToken)
    {
      ServiceResult<UserViewModel> res = new ServiceResult<UserViewModel>();

      var tokenRes = TokenHelper.TokenCoz(accessToken);
      
      if (tokenRes.StatusCode == ResultCode.Unauthorized)
      {
        res.StatusCode = tokenRes.StatusCode;
        res.Message = tokenRes.Message;
        return res;
      }
      res = _userService.Update(vm, tokenRes.Data.UserId);
      return res;
    }

    [HttpPut]
    [Route("UpdatePassword")]
    public ServiceResult UpdatePassword(string password, int userId, string accessToken)
    {
      ServiceResult res = new ServiceResult();

      var tokenRes = TokenHelper.TokenCoz(accessToken);

      if (tokenRes.StatusCode == ResultCode.Unauthorized)
      {
        res.StatusCode = tokenRes.StatusCode;
        res.Message = tokenRes.Message;
        return res;
      }
      res = _userService.UpdatePassword(password, userId, tokenRes.Data.UserId);
      return res;
    }

    //Tokenları tuttuğumuz UserTicket tablosundanda silmeli miyiz?
    [HttpDelete]
    [Route("DeleteWorker")]
    public ServiceResult DeleteWorker(int id,string accessToken)
    {
      ServiceResult res = new ServiceResult();

      //var tokenRes = TokenHelper.TokenCoz(accessToken);

      //if (tokenRes.StatusCode == ResultCode.Unauthorized)
      //{
      //  res.StatusCode = tokenRes.StatusCode;
      //  res.Message = tokenRes.Message;
      //  return res;
      //}
      res = _userService.Delete(id);
      return res;

    }
  }
}

