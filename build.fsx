#r @"packages/Fake/tools/FakeLib.dll"
#r @"packages\FSharp.Management\lib\net40\FSharp.Management.dll"


open System
open System.IO
open FSharp.Management
open Fake
open Fake.MSBuildHelper


Environment.CurrentDirectory <- __SOURCE_DIRECTORY__


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
//    |>  MSBuild 
//            editorLibs 
//            "Rebuild"
//            [   
//                "Configuration"        , "Release"
//                "DebugSymbols"         , "false" 
//                "Optimize"             , "true"  
//                "Verbosity"            , "some"  
//                "GenerateDocumentation", "false" 
//            ]
    |>  MSBuildRelease editorLibs "Rebuild"
    |> ignore
)


"Clean"
    ==> "Build"

"EditorClean"
    ==> "EditorExt"


let argvs = fsi.CommandLineArgs
match argvs.Length with
| 1 -> RunTargetOrDefault "Build"
| _ -> RunTargetOrDefault argvs.[1]