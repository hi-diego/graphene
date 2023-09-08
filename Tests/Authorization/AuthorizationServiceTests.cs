using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Graphene;
using Graphene.Services;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Tests.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationServiceTests
    {
        string jsonString =
            @"{
              ""Date"": ""2019-08-01T00:00:00"",
              ""Temperature"": 25,
              ""Summary"": ""Hot"",
              ""DatesAvailable"": [
                ""2019-08-01T00:00:00"",
                ""2019-08-02T00:00:00""
              ],
              ""TemperatureRanges"": {
                  ""Cold"": {
                      ""High"": 20,
                      ""Low"": -10
                  },
                  ""Hot"": {
                      ""High"": 60,
                      ""Low"": 20
                  }
              }
            }
            ";
        string jsonString2 =
            @"{
              ""Date"": ""2019-08-01T00:00:00"",
              ""Temperature"": 25,
              ""Summary"": ""Cold"",
              ""TestArray"": [1, 2, 3],
              ""DatesAvailable"": [
                ""2019-08-01T00:00:00"",
                ""2019-08-02T00:00:00""
              ],
              ""TemperatureRanges"": {
                  ""Cold"": {
                      ""High"": 0,
                      ""Low"": 0
                  },
                  ""Hot"": {
                      ""High"": 0,
                      ""Low"": 0
                  }
              }
            }
            ";
        string jsonResult = @"{""Date"":""2019-08-01T00:00:00"",""Temperature"":25,""Summary"":""Cold"",""TestArray"":[1,2,3],""DatesAvailable"":[""2019-08-01T00:00:00"",""2019-08-02T00:00:00""],""TemperatureRanges"":{""Cold"":{""High"":0,""Low"":0},""Hot"":{""High"":0,""Low"":0}}}";

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanMergeJson()
        {
            // Arrange
            var user = new Graphene.Entities.Authenticable();
            var newUser = new Graphene.Entities.Authenticable();
            newUser.Identifier = "hi@diego.pro";
            newUser.Id = 7078;
            var serializerOptions = new JsonSerializerOptions() { WriteIndented = false };

            // Act
            var mergedUser = (Graphene.Entities.Authenticable) user.Update(newUser, serializerOptions);
            var mergedJson = Graphene.Entities.BaseEntity.SimpleObjectMerge(jsonString, jsonString2);

            // Assert
            Assert.Equal(mergedUser.Identifier, newUser.Identifier);
            Assert.Equal(mergedUser.Id, 7078);
            Assert.Equal(jsonResult, mergedJson);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        //public async Task<bool> IsAuthorized(Entity entityInstance, IAuthorizable? user, IGrapheneDatabaseContext context)
        //{
        //    if (Expression == null) return true;
        //    var query = Graphene.Graph.Graph.GetSet<IAuthorizable>(context).Where(Expression, entityInstance);
        //    if (user != null) query = query.Where($"user => user.Id == {user.Id}");
        //    return await query.AsNoTracking().CountAsync() > 0;
        //}
    }
}
