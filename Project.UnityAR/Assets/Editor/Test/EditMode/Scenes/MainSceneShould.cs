using System.Collections;
using System.Collections.Generic;
using Components;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.Tests.Scenes
{
    [TestFixture]
    public class MainSceneShould
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        }

        [Test]
        public void contain_image_information_display()
        {
            var component = GameObject.FindObjectOfType<TrackedImageInformationDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.imageTrackedLabel);
            Assert.NotNull(component.idTrackedLabel);
        }

        [Test]
        public void contain_track_image_display()
        {
            var component = GameObject.FindObjectOfType<TrackedImageDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.trackImageManager);
            Assert.NotNull(component.rawImage);
        }

        [Test]
        public void contain_tracked_recognition_display()
        {
            var component = GameObject.FindObjectOfType<TrackedRecognitionDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.recognitionLabel);
            Assert.NotNull(component.animator);
        }

        [Test]
        public void contain_tracked_take_screen_display()
        {
            var component = GameObject.FindObjectOfType<TrackedTakeScreenDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.trackImageManager);
            Assert.NotNull(component.rawImage);
        }

        [Test]
        public void contain_tracked_image_input()
        {
            var component = GameObject.FindObjectOfType<TrackedImageInput>();
            Assert.NotNull(component);
            Assert.NotNull(component.trackedImageManager);
            Assert.NotNull(component.arCamera);
            Assert.NotNull(component.gameCmdFactory);
            Assert.NotNull(component.trackData);
        }

        [Test]
        public void contain_tracked_button_display()
        {
            var component = GameObject.FindObjectOfType<TrackedButtonDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.trackImageManager);
            Assert.NotNull(component.trackInput);
            Assert.NotNull(component.trackLabel);
        }

        [Test]
        public void contain_tracked_manager_image_display()
        {
            var component = GameObject.FindObjectOfType<TrackedManagerImageDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.ARTrackedImageManager);
            Assert.NotNull(component.trackData);
        }

        [Test]
        public void contain_tracked_manager_controller_display()
        {
            var component = GameObject.FindObjectOfType<TrackedManagerControllerDisplay>();
            Assert.NotNull(component);
            Assert.NotNull(component.trackedImageManager);
            Assert.NotNull(component.trackData);
        }
    }
}
