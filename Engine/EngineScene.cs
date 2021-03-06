using System;
using System.Collections.Generic;
using Win = System.Windows;

namespace PracticeWork.Engine
{
    public record EngineSceneConfiguration(string ConfigKey, Type ConfigType, object ConfigValue);
    public record EngineObjectWithIncludes(Engine.EngineObject Object, List<string> IncludeChildren);
    
    public interface IEngineScene 
    {
        List<EngineObject> GetSceneObjects<TObject>() where TObject : EngineObject;
        EngineObject? GetSceneObject(string required_object_name);
        EngineSceneConfiguration? GetSceneConfiguration(string object_name, string config_name);

        void ExitSceneHandler();
        void RunSceneHandler(double update_delay, System.Action on_exit_operation);
        System.Drawing.Size SceneSize { get; }
    }

    public sealed class EngineSceneBuilder : System.Object
    {
        private readonly List<EngineObjectWithIncludes> scene_initial_children = new(0);
        private readonly Dictionary<string, List<EngineSceneConfiguration>> scene_configuration = new(0);
        
        public Win::Forms.Panel ScenePanel { get; init; }
        public string SceneName { get; init; }

        public EngineSceneBuilder(string scene_name, Win::Forms.Panel scene_panel) 
            => (this.SceneName, this.ScenePanel) = (scene_name, scene_panel);

        public EngineSceneBuilder RegisterSceneConfiguration<TConfig>(string object_name, string key, TConfig value)
        {
            if (this.scene_configuration.ContainsKey(object_name) == true)
            {
                if (this.scene_configuration[object_name].Find((target) => target.ConfigKey == key) != null)
                    throw new Exception("Configuration allready set: " + object_name + "; " + key);
                this.scene_configuration[object_name].Add(new EngineSceneConfiguration(key, typeof(TConfig), value!));
            }    
            else this.scene_configuration.Add(object_name, new() { new(key, typeof(TConfig), value!) });
            return this;
        }

        public EngineSceneBuilder RegisterSceneChild<TObject>(string name, List<string> children) where TObject: EngineObject
        {
            System.Reflection.ConstructorInfo? selected_constructor = null;
            foreach (var contructor in typeof(TObject).GetConstructors())
            {
                var constructor_attributes = contructor.GetCustomAttributes(typeof(Engine.EngineObjectConstructorSelecter), true);
                if (constructor_attributes.Length != 0) selected_constructor = contructor;
            }
            if (selected_constructor == null) throw new Exception("Not found ctor for engine object");

            TObject? @object = selected_constructor.Invoke(new object[] { name }) as TObject;
            if (this.scene_initial_children.FindAll((EngineObjectWithIncludes target)
                => target.Object.Equals(@object)).Count > 0) throw new Exception("Allready registred");

            this.scene_initial_children.Add(new(@object!, children));
            return this;
        }

        private void SetSceneObjectConfig(EngineObjectWithIncludes target_object)
        {
            var object_config_list = Enumerable.Where(this.scene_configuration, (target) => target.Key
                == target_object.Object.ObjectName).ToList();

            if (object_config_list.Count == 0) return;
            foreach (System.Reflection.PropertyInfo infos in target_object.Object.GetType().GetProperties())
            {
                var property_attr = infos.GetCustomAttributes(typeof(Engine.EngineObjectImportConfiguration), true);
                if (property_attr.Length == 0) continue;

                var property_name = (property_attr[0] as EngineObjectImportConfiguration)!.PropertyName;
                var property_value = object_config_list[0].Value.Find(config => config.ConfigKey == property_name);

                if(property_value != null)
                {
                    try { infos.SetValue(target_object.Object, property_value.ConfigValue); }
                    catch (System.Exception error) { Console.WriteLine(error.Message); }
                }
            }
        }

        public IEngineScene BuildScene()
        {
            Engine.IEngineScene scene_instance = new EngineScene(ScenePanel, scene_initial_children.Select(
                (item) => item.Object).ToList(), scene_configuration) { SceneName = this.SceneName };

            this.scene_initial_children.ForEach(delegate(EngineObjectWithIncludes target_object) 
            {
                this.SetSceneObjectConfig(target_object);
                target_object.IncludeChildren.ForEach(delegate(string include_name)
                {
                    Engine.EngineObject? required_object = scene_instance.GetSceneObject(include_name);
                    if (required_object != null) target_object.Object.ConnectLinkToChildren(required_object);
                });
                target_object.Object.ConnectLinkToScene(scene_instance);
            });
            return scene_instance;
        }
    }

    public delegate void UpdateOperationHandling(Engine.IEngineScene scene_link);

    public sealed class EngineScene : IEngineScene
    {
        private readonly List<Engine.EngineObject> registred_scene_children;
        private readonly Dictionary<string, List<EngineSceneConfiguration>> registred_scene_configuration;

        private readonly Win::Forms.Panel panel_node_instance;
        private event UpdateOperationHandling? UpdateHandling = default;

        public System.String SceneName { get; init; } = "Scene";
        public System.Boolean IsExit { get; private set; } = false;

        public Size SceneSize { get => this.panel_node_instance.Size; }

        public EngineScene(Win::Forms.Panel panel, List<EngineObject> scene_children,
            Dictionary<string, List<EngineSceneConfiguration>> scene_configuration)
        {
            (this.registred_scene_children, this.registred_scene_configuration, this.panel_node_instance) 
                = (scene_children, scene_configuration, panel);
            this.InitializeSceneHandling();
        }

        private object painter_locker = new();
        private void InitializeSceneHandling()
        {
            this.registred_scene_children.Where((EngineObject target_object) => target_object is EngineInputController)
                .ToList().ForEach(delegate (EngineObject selected_object)
                {
                    EngineInputController? controller = (selected_object as EngineInputController);
                    (this.panel_node_instance as Control).KeyDown += (sender, arg) => controller?.KeyInputOperation(arg);
                    (this.panel_node_instance as Control).KeyUp += (sender, arg) => controller?.KeyReleaseOperation(arg);

                    (this.panel_node_instance as Control).MouseMove += (sender, arg) => controller?.MouseMoveOperation(arg);
                    (this.panel_node_instance as Control).MouseDown += (sender, arg) => controller?.MouseInputOperation(arg);
                    (this.panel_node_instance as Control).MouseUp += (sender, arg) => controller?.MouseReleaseOperation(arg);
                });

            //this.registred_scene_children.ForEach((EngineObject target_object)
            //    => (this.panel_node_instance.Paint) += (sender, arg) => { target_object?.UpdateOperation(arg.Graphics); });

            this.panel_node_instance.Paint += delegate (object? sender, PaintEventArgs arg)
            {
                this.registred_scene_children.ForEach((EngineObject target_object) =>
                {
                    lock(this.painter_locker) { target_object?.PaintingOperation(arg.Graphics); }
                });
            };

            this.registred_scene_children.ForEach((target_object) => this.UpdateHandling += target_object.UpdateOperation);
        }

        public EngineObject? GetSceneObject(string required_object_name) => this.registred_scene_children.Find(
            (Engine.EngineObject target) => target.ObjectName == required_object_name);

        public List<EngineObject> GetSceneObjects<TObject>() where TObject : Engine.EngineObject
            => this.registred_scene_children.FindAll((target) => (target as TObject) != null);

        public EngineSceneConfiguration? GetSceneConfiguration(string object_name, string config_name)
        {
            if (this.registred_scene_configuration.TryGetValue(object_name, out var config_list)) 
            {
                return config_list.Find((EngineSceneConfiguration target_config) => target_config.ConfigKey == config_name);
            }
            else throw new Exception("Cannot find required object");
        }

        private object timer_locker = new();
        public void RunSceneHandler(double update_delay, Action on_exit_operation)
        {
            Win::Forms.Timer scene_handling_timer = new() { Interval = (int)update_delay };
            scene_handling_timer.Tick += new EventHandler(delegate (object? sender, EventArgs args)
            {
                if(this.IsExit) { scene_handling_timer.Stop(); on_exit_operation(); return; }

                lock (this.timer_locker)
                {
                    if(this.UpdateHandling?.GetInvocationList().Length != 0) this.UpdateHandling!(this);

                    this.panel_node_instance.Focus();
                    this.panel_node_instance.Invalidate();
                }
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            });
            this.registred_scene_children.ForEach((target_record) => target_record.InitialOperation(this));
            scene_handling_timer.Start();
        }

        public void ExitSceneHandler() => this.IsExit = true;
    }
}
