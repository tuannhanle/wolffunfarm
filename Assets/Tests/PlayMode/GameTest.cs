using System;
using App.Scripts.Domains.Core;
using App.Scripts.Domains.Models;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayMode
{
    [TestFixture]
    public class GameTest : ICheat
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
        
        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_1_CheckDependencyProviderExist()
        {
            Assert.NotNull(DependencyProvider.Instance );
            yield return null;
        }

        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_2_CheckStatManagerExist()
        {

            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            Assert.NotNull(statManager);
            yield return null;
        }
        
        
        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_3_CheckStatInStatManager()
        {
            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            Assert.AreEqual(0, statManager.Gold.Amount);
            yield return null;
        }
        
        
        
        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_4_TestUpgradeToolToLevel1()
        {
            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            statManager.CheatGoldAmount(this);
            var x = new ToolManager();
            x.UpgradeTool();
            Assert.AreEqual(1, x.GetToolLevel);
            yield return null;
        }

        public int GoldAmount { get; set; } = 1000;
    }
}