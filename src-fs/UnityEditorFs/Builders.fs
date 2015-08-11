namespace UnityEditorFs

open System
open System.Text
open UnityEngine
open UnityEditor 
open Microsoft.FSharp.Control


[<AutoOpen>]
module Builders =



    let inline (>>=) expr lambda = expr |> lambda

    type Layout(fn:unit->unit) = 
        member __.Render () = fn()



    type GUIHorizontalBuilder([<ParamArray>] options,?style:GUIStyle) =
        let mutable rect = Rect()

        new() = GUIHorizontalBuilder()

        [<CustomOperation("Bounds", MaintainsVariableSpace = true)>]
        member __.Bounds (fn:Rect->unit) = fn rect 

        member __.Delay fn = 
            if style.IsNone then rect <- EditorGUILayout.BeginHorizontal(options)
            else rect <- EditorGUILayout.BeginHorizontal(style.Value,options)
            fn()

        member __.Zero x =  GUILayout.EndHorizontal(); x

        member __.Combine(a,b) = a; b           


    type GUIVerticalBuilder([<ParamArray>] options,?style:GUIStyle) =
        let mutable rect = Rect()

        new() = GUIVerticalBuilder()

        [<CustomOperation("Bounds", MaintainsVariableSpace = true)>]
        member __.Bounds (fn:Rect->unit) = fn rect 

        member __.Delay fn = 
            if  style.IsNone 
            then rect <- EditorGUILayout.BeginVertical(options)
            else rect <- EditorGUILayout.BeginVertical(style.Value,options)
            fn()

        member __.Zero x =  GUILayout.EndVertical(); x

        member __.Combine(a,b) = a; b        


    
    let guiVert = GUIVerticalBuilder()
    let guiHorz = GUIHorizontalBuilder()



//    let bind (f:'a -> StringBuilder) (sb:StringBuilder) =
//        

    type StringBuilderCE () =
        //member __.Yield (txt : string) = txt
        //member __.Yield (c : char) = StringItem(c.ToString())

        member __.Bind(expr, lambda) =  expr |> lambda

        member __.Combine(f:StringBuilder,g:StringBuilder)  = f.Append(string g)
        member __.Combine(f:StringBuilder,g:string       )  = f.Append(g)
        member __.Combine(f:string       ,g:StringBuilder)  = g.Insert(0,f)
        member __.Combine(f:string       ,g:string       )  = StringBuilder(f).Append(g)

        member __.Delay f = f

        member __.Run   f = f()

        member __.Zero () = ""

        member __.ReturnFrom(x) = x

        member this.While(guard, body) =
            if not (guard()) 
            then this.Zero() 
            else this.Bind( body(), fun () -> 
                this.While(guard, body)) 

        member this.TryFinally(body, compensation) =
            try this.ReturnFrom(body())
            finally compensation() 

        member this.Using(disposable:#System.IDisposable, body) =
            let body' = fun () -> body disposable
            this.TryFinally(body', fun () -> 
                match disposable with 
                    | null -> () 
                    | disp -> disp.Dispose())

        member this.For(sequence:#seq<_>, body) =
            this.Using(sequence.GetEnumerator(),fun enum -> 
                this.While(enum.MoveNext, 
                    this.Delay(fun () -> body enum.Current)))

    
