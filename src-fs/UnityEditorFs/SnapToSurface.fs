namespace UnityEditorFs

open UnityEngine
open UnityEditor
open Builders

module SnapToSurface =
    [<InitializeOnLoad>]
    type SnapWindow () =
        inherit EditorWindow() 

        [<MenuItem("FsExtensions/Snap To Surface")>]
        static member ShowWindow() =
            EditorWindow.GetWindow<SnapWindow>().titleContent <- GUIContent "Snap to Surface"
 
        static member Drop (dir:Vector3) =
            Selection.gameObjects |> Array.iter( fun go ->
                let collider = go.GetComponent<Collider>()
                sprintf "Collider %A" collider |> debuglog
                if not<|isNull collider && not(collider :? CharacterController) then
                    debuglog "There is a collider"
                    let rigidBody, addedRigidBody =
                        sprintf "getcomponent rigidbody isNull %A" (isNull(go.GetComponent<Rigidbody>())) |> debuglog
                        sprintf "getcomponent rigidbody = null %A" (null =(go.GetComponent<Rigidbody>())) |> debuglog
                        let rb = go.GetComponent<Rigidbody>()
                        sprintf "getcomponent rigidbody %A" rb |> debuglog
                        sprintf "isNull rb = %A" (isNull rb) |> debuglog
                        sprintf "null = rb = %A" (null = rb) |> debuglog
                        if isNull rb then 
                            debuglog "tried to add rigidbody"
                            (go.AddComponent<Rigidbody>(), true)
                            
                        else
                            (rb, false)
                    sprintf "Rigidbody %A, addedRigidBody %A" rigidBody addedRigidBody
                    |> debuglog

                    let mutable hit = RaycastHit()
                    if rigidBody.SweepTest(dir,&hit) then 
                        go.transform.position <- dir * hit.distance + go.transform.position
                    if addedRigidBody then 
                        GameObject.DestroyImmediate rigidBody
                else
                    debuglog "There is no collider"
                    let savedLayer = go.layer
                    go.layer <- 2
                    let mutable hit = RaycastHit()                    
                    if Physics.Raycast(go.transform.position,dir,&hit) 
                    then go.transform.position <- hit.point
                    go.layer <- savedLayer
            )

        member __.OnGUI() = 
            horzBlock (fun _ -> 
                if textButton "X" then SnapWindow.Drop(vec3 1.f 0.f 0.f )
                if textButton "Y" then SnapWindow.Drop(vec3 0.f 1.f 0.f )
                if textButton "Z" then SnapWindow.Drop(vec3 0.f 0.f 1.f )  
            )

            horzBlock(fun _ ->
               if textButton "-X" then SnapWindow.Drop(vec3 -1.f  0.f  0.f )
               if textButton "-Y" then SnapWindow.Drop(vec3  0.f -1.f  0.f )
               if textButton "-Z" then SnapWindow.Drop(vec3  0.f  0.f -1.f )  
            )
            

//            using (new HorizontalBlock()) (fun _ -> 
//                if textButton "X" then SnapWindow.Drop(vec3 1.f 0.f 0.f )
//                if textButton "Y" then SnapWindow.Drop(vec3 0.f 1.f 0.f )
//                if textButton "Z" then SnapWindow.Drop(vec3 0.f 0.f 1.f )  
//            )
//
//            using (new HorizontalBlock()) (fun _ ->
//               if textButton "-X" then SnapWindow.Drop(vec3 -1.f  0.f  0.f )
//               if textButton "-Y" then SnapWindow.Drop(vec3  0.f -1.f  0.f )
//               if textButton "-Z" then SnapWindow.Drop(vec3  0.f  0.f -1.f )  
//            )
//            

