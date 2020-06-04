using Onbox.Store.V7;

namespace WpfApp1
{
    public class PersonInitAction : IStoreAction<Person>
    {
        public string GetActionName()
        {
            return "Initialize Person";
        }

        public string GetActionPath()
        {
            return null;
        }
    }

    public class AnyAction : IStoreAction<Person>
    {
        public string GetActionName()
        {
            return "Any Action";
        }

        public string GetActionPath()
        {
            return null;
        }
    }


    public class SelectPetAction : IStoreAction<Pet>
    {
        public string GetActionName()
        {
            return "Select Pet";
        }

        public string GetActionPath()
        {
            return "Pet";
        }
    }

    public class EditPetAction : IStoreAction<Pet>
    {
        public string GetActionName()
        {
            return "Edit Pet";
        }

        public string GetActionPath()
        {
            return "Pet";
        }
    }

    public class CreatePetAction : IStoreAction<Pet>
    {
        public string GetActionName()
        {
            return "Add Pet";
        }

        public string GetActionPath()
        {
            return "Pet";
        }
    }
}
