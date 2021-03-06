﻿using MatchmakingService.DataContext;
using MatchmakingService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchmakingService.Services.Repositories
{
    // Update the fucking interface!!!
    public class UserMatchRepository : Repository<UserMatch>, IUserMatchRepository
    {
        public UserMatchRepository(MatchmakingContext matchmakingContext) : base(matchmakingContext) { }

        public MatchmakingContext MatchmakingContext
        {
            get
            {
                return base.Context as MatchmakingContext;
            }
        }

        public UserInfo FindRandomUser(Guid currentUserFK)
        {
            UserInfo currentUser = MatchmakingContext.UserInfos
                .FirstOrDefault(x=>x.IdentityFK == currentUserFK);
            UserInfo potentialMatch = MatchmakingContext.UserInfos //check if both users exist and user1 wants to match with user2 OR they havent decided/seen each other yet
                .Include(x => x.Matches.Where(y => (y.UserInfo == x && y.User2 == currentUser && y.FirstSelection == true && y.IsAMatch == null) || y == null))
                .Where(x => x.IdentityFK != currentUser.IdentityFK)
                .FirstOrDefault();

            return potentialMatch;
        }

        public bool SaveMatchChoice(Guid currentUserId, Guid potentialMatchUserId, bool userMatch)
        {
            var currentUser = MatchmakingContext.UserInfos.FirstOrDefault(x => x.IdentityFK == currentUserId);
            var potentialMatchUser = MatchmakingContext.UserInfos.Include(x => x.Matches).FirstOrDefault(x => x.IdentityFK == potentialMatchUserId);

            if (potentialMatchUser.Matches.Where(x => x.UserInfo == potentialMatchUser && x.User2 == currentUser && x.FirstSelection == true).Count() == 1)
            {
                UserMatch match = potentialMatchUser.Matches.Where(x => x.UserInfo == potentialMatchUser && x.User2 == currentUser && x.FirstSelection == true).FirstOrDefault();
                match.IsAMatch = userMatch;
                MatchmakingContext.Update(match);
            }
            else
            {
                UserMatch newUserMatch = new UserMatch { UserInfo = currentUser, User2 = potentialMatchUser, FirstSelection = userMatch } ;
                currentUser.Matches.Add(newUserMatch);
            }
            var state = MatchmakingContext.SaveChanges();
            return state == 1 ? true : false;

        }
    }
}