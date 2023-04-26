
using App.Scripts.Domains.Models;
using App.Scripts.Domains.Core;
using NUnit.Framework;

[TestFixture]
public class GameManagerTests
{

    [Test]
    public void IncreaseScore_ShouldIncreasePlayerScoreByAmount()
    {
        // Arrange
        var player = new Player { Name = "John Doe", Score = 0 };
        var gameManager = new GameManager(player);

        // Act
        gameManager.IncreaseScore(100);

        // Assert
        Assert.AreEqual(100, player.Score);
    }
    
}



