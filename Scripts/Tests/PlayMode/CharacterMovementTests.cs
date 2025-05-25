using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterMovementTests
{
    [UnityTest]
    public IEnumerator PlayerMovesRight_WhenInputIsGiven()
    {
        var playerGO = new GameObject();
        playerGO.AddComponent<Rigidbody>();
        var movement = playerGO.AddComponent<PlayerMovement>();
        movement.speed = 5f;

        Vector3 startPos = playerGO.transform.position;

        playerGO.transform.position += Vector3.right * movement.speed * Time.deltaTime;

        yield return null;

        Assert.Greater(playerGO.transform.position.x, startPos.x);

        Object.Destroy(playerGO);
    }
}