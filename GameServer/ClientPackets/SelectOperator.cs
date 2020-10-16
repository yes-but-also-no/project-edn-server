using System;
using System.Linq;
using Data;
using GameServer.ServerPackets;
using Microsoft.EntityFrameworkCore;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Sent when the client wishes to select their operator
    /// </summary>
    public class SelectOperator : ClientBasePacket
    {
        /// <summary>
        /// The operator they wish to select
        /// </summary>
        private readonly int _operatorId;
        
        public SelectOperator(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("INT - {0}", GetInt()); // Unknown

            _operatorId = GetInt();
        }

        public override string GetType()
        {
            return "SELECT_OPERATOR";
        }

        protected override void RunImpl()
        {
            // TODO: Actually select operator
            var client = GetClient();

            using (var db = new ExteelContext())
            {
                // Get tracked entity
                var user = db.Users
                    .Single(u => u.Id == client.User.Id);

                // Update operator
                user.OperatorId = _operatorId;

                // Save to DB
                db.SaveChanges();
            }
            
            // Update user
            GetClient().UpdateUserFromDatabase();

            client.SendPacket(new SelectOperatorInfo(_operatorId));
        }
    }
}