namespace UnityEditorFs

open UnityEditor
open UnityEngine

module Locks =

    // Strings used to define the menu item heirarchy that will be used in MenuItem attributes

    let menuRoot                  = "GameObject/UnityLock"
    let lockMenuItem              = menuRoot+/"Lock GameObject"
    let lockRecursivelyMenuItem   = menuRoot+/"Lock GameObject and Children %#1"
    let unlockMenuItem            = menuRoot+/"Unlock GameObject"
    let unlockRecursivelyMenuItem = menuRoot+/"Unlock GameObject and Children"

    let showIconPrefKey           = "UnityLock_ShowIcon"
    let addUndoRedoPrefKey        = "UnityLock_EnableUndoRedo"
    let disableSelectionPrefKey   = "UnityLock_DisableSelection"


    let private setEditorKey key flag =
        if EditorPrefs.HasKey key then EditorPrefs.SetBool (key,flag)

    let UnityLock () =
        setEditorKey showIconPrefKey true
        setEditorKey addUndoRedoPrefKey true
        setEditorKey disableSelectionPrefKey false


type UnityLock() =
    inherit EditorWindow()
                                     
    [<MenuItem("FsExtensions/UnityLock Preferences")>]
    static member ShowPreferences() =
        let window = EditorWindow.GetWindow<UnityLock>()
        window.minSize <- vec2 275.0f 300.f
        window.Show()

    member __.x = ()

    member __.OnGui():unit =
        EditorGUILayout.BeginHorizontal()|> ignore
        EditorGUILayout.EndHorizontal()


    