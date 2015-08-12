#r @"packages/Fake/tools/FakeLib.dll"
#r @"packages\FSharp.Management\lib\net40\FSharp.Management.dll"


open System
open System.IO
open System.Diagnostics
open FSharp.Management
open Fake
open Fake.MSBuildHelper


Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

let (+/) path1 path2 = Path.Combine(path1, path2)
let argstr ls = String.concat " " ls

let run cmd args =
    let pinfo = ProcessStartInfo cmd
    pinfo.RedirectStandardOutput <- true
    pinfo.UseShellExecute        <- false
    pinfo.Arguments              <- args
    let proc = System.Diagnostics.Process.Start (pinfo)
    proc.WaitForExit()

let inline quote str = String.Concat["\"";str;"\""]
let PathName path    = FullName path |> quote




type Relative = RelativePath<".",true>

let solutionFile  = Relative.``src-fs``.``UnityEditorFs.sln``
let editorLibs    = Relative.Assets.Editor.Path
let gizmoDir      = Relative.Assets.Gizmos.Path  

Target "Clean"( fun _ -> 
    trace "Cleaning up detritus"
    CleanDirs ["src-fs/UnityEditorFs/bin";"src-fs/UnityEditorFs/temp"]
)

Target "Trace"( fun _ ->
    trace "I'd rather FAKE it than MAKE it"
)

Target "Build"( fun _ ->
    trace "Building project files"
    !! solutionFile
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)


Target "EditorClean"( fun _ -> 
    trace "Cleaning Extension dlls from Assets/Editor"
    CleanDir editorLibs
)


Target "EditorExt"( fun _ ->
    trace "Building project files for Unity Engine"
    !! solutionFile

    |>  MSBuildRelease "" "Rebuild"
//    |>  MSBuildRelease ("Assets"+/"libs") "Rebuild"
    |> ignore

    !! "src-fs/UnityEditorFs/bin/Release/*.dll"
    |> CopyFiles editorLibs
    

)


"Clean"
    ==> "Build"

"EditorClean"
    ==> "EditorExt"


let argvs = fsi.CommandLineArgs
match argvs.Length with
| 1 -> RunTargetOrDefault "Build"
| _ -> RunTargetOrDefault argvs.[1]