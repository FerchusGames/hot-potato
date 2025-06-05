using System.Threading;
using Character.Controllers;
using Character.Models;
using Character.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.AddressableLoader;
using Utils.DataAPI;

public struct CharacterDataByStyleName
{
    public string styleName;
}

public class GameApp : IGameApp
{
    private CancellationTokenSource gameToken;

    public GameApp()
    {
        gameToken = new CancellationTokenSource();
    }
    
    public async UniTaskVoid StartApp()
    {
        //var characterView = await AddressableLoader.InstantiateAsync<ICharacterView>("basePlayer");
        //ICharacterData characterData = new CharacterDataDummy();

        const string getCharacterDataQueryByStyleName = @"query CharacterDataByStyleName($styleNameId: String!) {
          CharacterDataByStyleName(styleNameId: $styleNameId) {
            id
            color
            styleName {
              id
              styleName
              priority
            }
          }
        }";

        const string getCharacterDataWitHighestStylePriority = @"query CharacterDataWitHighestStylePriority {
          CharacterDataWitHighestStylePriority {
            id
            color
            styleName {
              id
              styleName
              priority
            }
          }
        }";
        
        

        /*var characterDataByStyleNameVariables = new CharacterDataByStyleName()
        {
            styleName = "basePlayer"
        };*/

        var fullQueryCharacterData = new GraphQlQuery()
        {
            query = getCharacterDataWitHighestStylePriority,
            variables = null
        };
        
        var characterData = await GraphqlUtils.GetModel<CharacterData>(fullQueryCharacterData, 
            "CharacterDataWitHighestStylePriority", 
            gameToken.Token);
        var characterStyle = characterData.StyleName.StyleName;
        var characterView = await AddressableLoader.InstantiateAsync<ICharacterView>(characterStyle);
        
        ICharacterBaseController characterBaseController =
            new CharacterBaseController(characterView, characterData, gameToken.Token);
    }

    public void Dispose()
    {
        gameToken.Cancel();
        gameToken?.Dispose();
        gameToken = null;
    }
}
