﻿using CateringPro.Models;
using CateringPro.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CateringPro.Repositories
{
    public interface ICompanyUserRepository
    {
        string GetCurrentCompany();
        Task<List<Company>> GetCurrentUsersCompaniesAsync(string userId);

        Task<bool> ChangeUserCompanyAsync(string userId, int companyid, ClaimsPrincipal claims);

        Task<List<UserGroups>> GetUserGroups(int companyId);

        Task<List<UserRoleViewModel>> GetRolesForUserAsync(CompanyUser user);
        Task<bool> PostUpdateUserAsync(CompanyUser user, bool isNew = false);
        Task<List<CompanyUser>> GetUserChilds(string userId, int companyId,bool onlyChild= false);
        Task<bool> PostUpdateChildUserAsync(CompanyUser childuser, CompanyUser parentuser);
        Task<AddBalanceViewModel> AddBalanceViewAsync(string userId);
        decimal GetUserBalance();
        Task<decimal> GetUserBalanceAsync();
        Task<bool> AddNewUserChild(string userId, int companyId);
        Task<List<Company>> GetCompaniesAsync();
        Task<List<AssignedCompanyEditViewModel>> GetAssignedCompaniesEdit(string userId);
        Task<bool> AddCompaniesToUserAsync(string userid, IList<int> companiesIds);
        string GetTokenForUser(CompanyUser user);
        string GenerateNewCardToken(string userid, string cardUid, bool addHash = false);
        Task<bool> SaveUserCardTokenAsync(string userId, string token);
        Task<UserSubGroupViewModel> GetSubGroupTree(int companyId);
        Task<List<AssignedCompanyEditViewModel>> GetAssignedEditCompanies(string userId);
        Task<int> GetUserCompanyCount(string userId);
        Task<List<int>> UserPermittedSubGroups(string userId, int companyid);
        Task<List<UserSubGroup>> GetUserSubGroups(int companyId);
        UpdateUserModel GetUpdateUserModel(CompanyUser user);
        List<int> GetUserSubGroups(string userId, int companyid);
        int GetUserSubGroupId(string userId);
        Task<bool> ValidateBasicAuthAsync(string val);
        Task<CompanyUserCompany> GetCurrentUserCompaniesUserAsync(string userId);
        Task<CompanyUserCompany> GetUserCompaniesUserAsync(string userId, int companyId);
        int GetTopLevelSubGroup();
        string GetUserSubGroupName(int subgroupid);
        List<SelectListItem> GetUserSubgroupsdWithEmptyList();
        List<SelectListItem> GetCompaniesWithEmptyList();
        int ValidateUserOnLogin(CompanyUser user);
        Task<CompanyUser> SaveUserTelegramAssociationAsync(long telegramid, string phonenumber);
    }
}
