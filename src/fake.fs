[<ReflectedDefinition>]
module Ionide.VSCode

open System
open System.Text.RegularExpressions
open FunScript
open FunScript.TypeScript
open FunScript.TypeScript.fs
open FunScript.TypeScript.child_process
open FunScript.TypeScript.vscode

open Ionide
open Ionide.VSCode

module FakeService =
    type BuildData = {Name : string; Start : DateTime; mutable End : DateTime option; Process : ChildProcess}

    let mutable private linuxPrefix = ""
    let mutable private command = ""
    let mutable private script = ""
    let mutable private BuildList = ResizeArray()

    let private loadParameters () =
        let p = workspace.Globals.getPath ()
        linuxPrefix <- Settings.loadOrDefault (fun s -> s.Fake.linuxPrefix ) "sh"
        command <- Settings.loadOrDefault (fun s -> p + "/" + s.Fake.command ) (if Process.isWin () then p + "/" + "build.cmd" else p + "/" + "build.sh")
        script <- Settings.loadOrDefault (fun s -> p + "/" + s.Fake.build )  (p + "/" + "build.fsx")
        ()

    let private startBuild target =
        let outputChannel = window.Globals.getOutputChannel "FAKE"
        outputChannel.clear ()
        let proc = Process.spawnWithNotification command linuxPrefix target outputChannel
        let data = {Name = (if target = "" then "Default" else target); Start = DateTime.Now; End = None; Process = proc}
        BuildList.Add data
        proc.on("exit",unbox<Function>(fun (code : string) ->
            if code ="0" then
                window.Globals.showInformationMessage "Build completed" |> ignore
            else
                window.Globals.showErrorMessage "Build failed" |> ignore
            data.End <- Some DateTime.Now)) |> ignore
        ()


    let cancelBuild target =
        let build = BuildList |> Seq.find (fun t -> t.Name = target)
        if Process.isWin () then
            Process.spawn "taskkill" "" ("/pid " + build.Process.pid.ToString() + " /f /t")
            |> ignore
        else
            build.Process.kill ()
        build.End <- Some DateTime.Now

    let buildHandle () =
        do loadParameters ()
        script
        |> Globals.readFileSync
        |> fun n -> (n.toString(), "Target \"([\\w.]+)\"")
        |> Regex.Matches
        |> Seq.cast<Match>
        |> Seq.toArray
        |> Array.map(fun m -> m.Groups.[1].Value)
        |> window.Globals.showQuickPick
        |> Promise.toPromise
        |> Promise.success startBuild

    let cancelHandle () =
        BuildList
        |> Seq.where (fun n -> n.End.IsNone)
        |> Seq.map (fun n -> n.Name)
        |> Seq.toArray
        |> window.Globals.showQuickPick
        |> Promise.toPromise
        |> Promise.success cancelBuild

    let defaultHandle () =
        do loadParameters ()
        do startBuild ""

type Fake() =
    member x.activate(state:obj) =
        let t = workspace.Globals.getPath ()
        commands.Globals.registerCommand("fake.fakeBuild", FakeService.buildHandle |> unbox<CommandCallback>) |> ignore
        commands.Globals.registerCommand("fake.cancelBuild", FakeService.cancelHandle |> unbox<CommandCallback>) |> ignore
        commands.Globals.registerCommand("fake.buildDefault", FakeService.defaultHandle |> unbox<CommandCallback>) |> ignore
        ()
