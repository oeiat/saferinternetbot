using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using mbit.common.cache;
using mbit.common.dal.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.Business.Mappings;
using oiat.saferinternetbot.Business.Services;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.LuisApi.ApiClient;

namespace oiat.saferinternetbot.Business.Test
{
    [TestClass]
    public class IntentServiceTests
    {
        private readonly IIntentService _intentService;
        private static readonly IMapper Mapper = MappingModule.Register();

        public IntentServiceTests()
        {
            var answerRepository = A.Fake<IRepository<Answer>>();
            A.CallTo(() => answerRepository.GetAllAsync()).Returns(Task.FromResult((ICollection<Answer>)Enumerable.Empty<Answer>()));

            _intentService = new IntentService(new MockLuisApiClient(), Mapper, new MemoryCacheService(0), answerRepository);
        }

        [TestMethod]
        public async Task Should_Get_2_Intents()
        {
            var items = await _intentService.GetAll();
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public async Task Should_Get_1_Intent()
        {
            var item = await _intentService.GetByName("bye");
            Assert.IsNotNull(item);
            Assert.AreEqual("bye", item.Name);
        }
    }
}
