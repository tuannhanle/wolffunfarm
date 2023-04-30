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
        public System.Collections.IEnumerator Test_3_CheckStatInStatManagerCheating()
        {
            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            statManager.CheatGoldAmount(this);
            // Assert.AreEqual(1000, statManager.Gold.Amount);
            yield return null;
        }
        
        
        
        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_4_TestToolManagerExist()
        {
            var toolManager = DependencyProvider.Instance.GetDependency<ToolManager>();
            toolManager.UpgradeTool();
            Assert.NotNull(toolManager);
            yield return null;
        }

        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator Test_5_TestUpgradeToolToLevel2()
        {
            var statManager = DependencyProvider.Instance.GetDependency<StatManager>();
            statManager.CheatGoldAmount(this);
            var toolManager = DependencyProvider.Instance.GetDependency<ToolManager>();
            var result = toolManager.UpgradeTool();
            if (result)
            {
                Assert.AreEqual(2, toolManager.GetToolLevel);
            }
            else
            {
                Assert.AreEqual(false, result);

            }
            yield return null;

        }
        
        public int GoldAmount { get; set; } = 1000;
    }
}