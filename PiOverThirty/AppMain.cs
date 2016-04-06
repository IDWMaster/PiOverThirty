using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Urho;
using Urho.Gui;
using Urho.Actions;

namespace PiOverThirty
{
    public class AppMain:Application
    {
        protected override void Start()
        {
            CreateScene();
            
        }
        async void CreateScene()
        {

            // Create a top-level scene, must add the Octree
            // to visualize any 3D content.
            var scene = new Scene();
            scene.CreateComponent<Octree>();
            // Box
            Node boxNode = scene.CreateChild();
            boxNode.Position = new Vector3(0, 0, 5);
            boxNode.Rotation = new Quaternion(60, 0, 30);
            boxNode.SetScale(0f);
            StaticModel modelObject = boxNode.CreateComponent<StaticModel>();
            modelObject.Model = ResourceCache.GetModel("Models/Box.mdl");
            // Light
            Node lightNode = scene.CreateChild(name: "light");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
            lightNode.CreateComponent<Light>();
            // Camera
            Node cameraNode = scene.CreateChild(name: "camera");
            Camera camera = cameraNode.CreateComponent<Camera>();
            // Viewport
            Renderer.SetViewport(0, new Viewport(scene, camera, null));
            // Perform some actions
            await boxNode.RunActionsAsync(
                new EaseBounceOut(new ScaleTo(duration: 1f, scale: 1)));
            await boxNode.RunActionsAsync(
                new RepeatForever(new RotateBy(duration: 1,
                    deltaAngleX: 90, deltaAngleY: 0, deltaAngleZ: 0)));
        }
    }
}