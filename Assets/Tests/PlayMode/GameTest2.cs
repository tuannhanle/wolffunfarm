using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode
{
    [TestFixture]
    public class GameTest2
    {
        private GameObject _go;
        private GameManager _gameManager;
        
        [SetUp]
        public void SetUp()
        {
            _go = GameObject.Instantiate(new GameObject());
            _gameManager = _go.AddComponent<GameManager>();
            _go.AddComponent<DependencyProvider>();

        }

        [Test]
        public void Test_1_SeedingStrawberry()
        {
            var workerManager = DependencyProvider.Instance.GetDependency<WorkerManager>();
            var dataLoader = DependencyProvider.Instance.GetDependency<DataLoader>();
            dataLoader.JobStorage = new();
            dataLoader.Push<Job>();
            workerManager.Assign(JobType.PutIn, "Strawberry");
            dataLoader.Push<Job>();
            Assert.AreEqual(true, dataLoader.JobStorage.Count > 0);
            
        } 
    }
}