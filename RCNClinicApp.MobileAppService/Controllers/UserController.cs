﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RCNClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryAsync<tbl_Users> repository;
        public UserController(IRepositoryAsync<tbl_Users> repository)
        {
            this.repository = repository;
        }


        

        [HttpGet("{username}/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<JsonR>> Get(string username,string password)
        {
            var tbl_Users =await repository.Get(c => c.UserName == username && c.Password == password && !c.IsDelete.GetValueOrDefault(false));
            if (tbl_Users == null)
            {
                return new JsonR
                {
                    Title = "error",
                    Message = "چنین کاربری وجود ندارد"
                };
            }

            return new JsonR
            {
                Title = "success",
                Message =tbl_Users.Id.ToString()
                
            };
        }

       

       
    }
}