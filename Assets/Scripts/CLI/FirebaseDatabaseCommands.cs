using Firebase.Database;
using Infrastructure;
using QFSW.QC;
using UnityEngine;

namespace CLI
{
    public class FirebaseDatabaseCommands : MonoBehaviour
    {
        private DatabaseReference _reference;


        private void Start()
        {
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        [Command("database.testSave")]
        public void TestSave(string message)
        {
            var testData = new TestData
            {
                name = "Alex",
                message = message
            };
            _reference.Child("Testing").SetRawJsonValueAsync(JsonUtility.ToJson(testData)).ContinueWith(x =>
            {
                if (x.IsCompleted) Debug.Log("Data sended");
            });
        }

        [Command("database.testLoad")]
        public void TestLoad()
        {
            _reference.Child("Testing").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Data received");
                    var snapshot = task.Result;
                    Debug.Log($"Message is: '{snapshot.Child("message").Value}'");
                }
            });
        }
    }
}