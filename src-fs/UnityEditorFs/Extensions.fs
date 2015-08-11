namespace UnityEditorFs
open System
open System.Text
open UnityEngine
open UnityEditor

[<AutoOpen>]
module Extensions =


    type StringBuilder with
        member sb.AppendWhen<'T> (predicate : unit -> bool, value : 'T) =
            if predicate () 
            then sb.Append value
            else sb

        member sb.AppendFormatWhen (predicate : unit -> bool, format, args : obj seq) =
            if predicate() 
            then sb.AppendFormat (format, args |> Seq.toArray)
            else sb

        member sb.Append (xs : #seq<_>, appender ) = Seq.fold appender sb xs        
        member sb.Append (xs : 'a list, appender ) = List.fold appender sb xs        
        member sb.Append (xs : 'a [], appender ) =   Array.fold appender sb xs        
        static member Append (xs : #seq<_>, appender, sb) = Seq.fold appender sb xs        
        static member Append (xs : 'a list, appender,sb ) = List.fold appender sb xs        
        static member Append (xs : 'a [], appender, sb ) = Array.fold appender sb xs        




