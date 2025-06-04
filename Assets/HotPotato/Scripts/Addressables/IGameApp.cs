using System;
using Cysharp.Threading.Tasks;

public interface IGameApp : IDisposable
{ 
    UniTaskVoid StartApp();
}