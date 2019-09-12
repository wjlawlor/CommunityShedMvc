using CommunityToolShedMvc.Models;
using System;
using System.Security.Principal;

namespace CommunityToolShedMvc.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity identity;
        private Person person;

        public CustomPrincipal(CustomIdentity identity, Person person)
        {
            this.identity = identity;
            this.person = person;
        }

        public Person Person
        {
            get
            {
                return person;
            }
        }

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(int communityId, string role)
        {
            bool roleFound = false;

            foreach (var commuinityRole in person.Roles)
            {
                if (commuinityRole.CommunityID == communityId)
                {
                    if ((role == "Approver" && commuinityRole.isApprover) ||
                        (role == "Reviewer" && commuinityRole.isReviewer) ||
                        (role == "Enforcer" && commuinityRole.isEnforcer))
                    {
                        roleFound = true;
                        break;
                    }
                }
            }

            return roleFound;
        }
    }
}