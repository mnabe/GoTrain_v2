﻿using MatchmakingService.DataContext;
using MatchmakingService.Helpers;
using MatchmakingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchmakingService.Services.Repositories
{
    // Update the fucking interface!!!
    public class UserMatchRepository : Repository<UserMatch>, IUserMatchRepository
    {
        public UserMatchRepository(MatchmakingContext matchmakingContext) : base(matchmakingContext)
        {

        }

        public MatchmakingContext MatchmakingContext
        {
            get
            {
                return base.Context as MatchmakingContext;
            }
        }

        public UserMatch GetMatch(Guid userId)
        {
            UserInfo user = MatchmakingContext.UserInfos.First(x => x.IdentityFK == userId);
            UserInfo otherUser = null;
            UserMatch potentialMatch = null;

            List<UserInfo> potentialMatchUsers = MatchmakingContext.UserInfos.ToList();
            potentialMatchUsers.Remove(user);

            // this list will contain the user itself, handle that in the looooop
            List<UserMatch> potentialMatches = MatchmakingHelpers.RemoveMatchDuplicates(potentialMatchUsers, user.Matches).ToList();


            int numOfUsers = potentialMatches.Count();

            for (int i = 0; i < numOfUsers; i++)
            {
                // Redo all below here. Go over code!
                otherUser = MatchmakingContext.UserInfos.ElementAt(i);
                if (otherUser.IdentityFK == userId)
                {
                    i++;
                    otherUser = MatchmakingContext.UserInfos.ElementAt(i);
                }


                
                //potentialMatch = MatchmakingContext.Matches[i]
                //potentialMatch = MatchmakingContext.Matches.FirstOrDefault(x => ((x.User2Id == otherUser.IdentityFK &&
                //                                                    x.User1Id == u1Id &&
                //                                                    x.User2Match == true &&
                //                                                    x.User1Match == null)) ||
                //                                                   (x.User1Id == otherUser.IdentityFK &&
                //                                                    x.User2Id == u1Id &&
                //                                                    x.User1Match == true &&
                //                                                    x.User2Match == null));
                if (potentialMatch != null)
                {
                    return potentialMatch;
                }
            }
            return null;
        }
    }
}

        //    public UserMatch GetMatch(Guid u1Id)
        //    {
        //        int numOfUsers = MatchmakingContext.UserInfos.Count();
        //        // other user has agreed to match or hasnt seen
        //        UserInfo otherUser = null;
        //        UserMatch potentialMatch = null;

        //        for (int i = 0; i < numOfUsers; i++)
        //        {
        //            otherUser = MatchmakingContext.UserInfos.FirstOrDefault(x => x.IdentityFK != u1Id);

        //            potentialMatch = MatchmakingContext.Matches.FirstOrDefault(x => ((x.User2Id == otherUser.IdentityFK &&
        //                                                                x.User1Id == u1Id &&
        //                                                                x.User2Match == true &&
        //                                                                x.User1Match == null)) ||
        //                                                               (x.User1Id == otherUser.IdentityFK &&
        //                                                                x.User2Id == u1Id &&
        //                                                                x.User1Match == true &&
        //                                                                x.User2Match == null));
        //            if(potentialMatch != null)
        //            {
        //                return potentialMatch;
        //            }
        //        }
        //        return new UserMatch();
        //    }
        //}