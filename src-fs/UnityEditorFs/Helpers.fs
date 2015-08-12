namespace UnityEditorFs
open System
open UnityEngine
open UnityEditor


[<AutoOpen>]
module Helpers =

    let ensureDirectory root folder =
        if AssetDatabase.IsValidFolder (root+/folder) then () else
        AssetDatabase.CreateFolder( root, folder) |> ignore




    type VerticalBlock ([<ParamArray>] options, ?style:GUIStyle) =
        do if style.IsSome 
            then GUILayout.BeginVertical(style.Value,options) |> ignore
            else GUILayout.BeginVertical(options) |> ignore
        
        interface IDisposable with 
            member __.Dispose() =  GUILayout.EndVertical()
    
        new() = new VerticalBlock([||])

    let vertBlock func = using (new VerticalBlock()) func


        
    type HorizontalBlock ([<ParamArray>] options, ?style:GUIStyle) =
        do if style.IsSome 
            then GUILayout.BeginHorizontal(style.Value,options) |> ignore
            else GUILayout.BeginHorizontal(options) |> ignore

        new() = new HorizontalBlock([||])

        interface IDisposable with 
            member __.Dispose() =  GUILayout.EndHorizontal()

    let horzBlock func = using (new HorizontalBlock()) func

