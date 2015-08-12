#nowarn "9"
namespace UnityEditorFs


open System
open System.Runtime.InteropServices
open UnityEngine
open UnityEditor


module TextBuffer =



    [<Struct;StructLayout(LayoutKind.Sequential);Serializable>]
    type TextBlock(text:string, style:GUIStyle) =
        member __.Style = style
        member __.Text  = text
        static member Empty = TextBlock()


        

    let t = TextEditor()

