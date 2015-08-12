namespace UnityEditorFs
open System
open UnityEngine
open UnityEditor

[<AutoOpen>]
module Prelude =
    /// Path join operator
    let inline (+/) a b = String.concat "" [a;"/";b]

    let inline private (+=) (x : byref<_>) y = x <- x + y

    let concat xs = String.concat "" xs

    let debuglog (msg:string) = Debug.Log msg


    let logthru format elm = sprintf format elm |>  Debug.Log; elm

    let isNull x = 
        let y = box x // In case of value types
        obj.ReferenceEquals (y, Unchecked.defaultof<_>) || y.Equals(Unchecked.defaultof<_>) 

//=======================
//  GUI ELEMENTS
//=======================

    let textButton  (txt:string) = GUILayout.Button(txt)
    let textButtonP (txt:string) ([<ParamArray>] options) = GUILayout.Button(txt,options)

    let textLabel  (txt:string) = GUILayout.Label txt
    let styleLabel (txt:string)(style:GUIStyle) = GUILayout.Label( txt,style)


    let textField (content:string) = 
        EditorGUILayout.TextField(content)
    let labelField (label:string) (content:string) = 
        EditorGUILayout.TextField(label, content)

    //
//===================================
//  CURRIED FUNCS FOR CONSTRUCTORS
//===================================

    /// Create a Vector2 struct
    let vec2 x y = Vector2(x,y)
    /// Create a Vector 3 struct
    let vec3 x y z = Vector3(x,y,z)



    
