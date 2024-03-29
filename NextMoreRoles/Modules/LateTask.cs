using System;
using System.Collections.Generic;

namespace NextMoreRoles.Modules
{
    //TownOfHostよりパクら.....いただきました、ありがとうございます！
    class LateTask
    {
        public string name;
        public float timer;
        public Action action;
        public static List<LateTask> Tasks = new();
        public static List<LateTask> AddTasks = new();
        public bool run(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                action();
                return true;
            }
            return false;
        }
        public LateTask(Action action, float time, string name = "No Name Task")
        {
            this.action = action;
            this.timer = time;
            this.name = name;
            AddTasks.Add(this);
        }

        //実行元:HarmonyPatches.HudManager.cs
        public static void Update(float deltaTime)
        {
            var TasksToRemove = new List<LateTask>();
            Tasks.ForEach((task) =>
            {
                if (task.run(deltaTime))
                {
                    TasksToRemove.Add(task);
                }
            });
            TasksToRemove.ForEach(task => Tasks.Remove(task));
            foreach (LateTask task in AddTasks)
            {
                Tasks.Add(task);
            }
            AddTasks = new List<LateTask>();
        }
    }
    /*public class LateUpdate
    {
        public static void Postfix(HudManager __instance)
        {
            LateTask.Update(Time.deltaTime);
        }
    }*/
}
