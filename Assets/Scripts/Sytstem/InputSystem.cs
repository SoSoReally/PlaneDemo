using UnityEngine;
using XInputDotNetPure;

namespace InputManager
{
    public class InputSystem :MonoBehaviour
    {
        private InputSystem() { }

        private  Mapping mapping = Mapping.Get;

        private GamePadState state;

        public GamePadState State { get { return state; } }

        private static InputSystem m_input;

        public static InputSystem Get
        {
            get
            {
                if (!m_input)
                {
                    m_input = new InputSystem();
                }
                return m_input;
            } }

        private void Awake()
        {
            m_input = this;
        }

        private void Update()
        {
            state = GamePad.GetState(PlayerIndex.One);
        }
        
        public  bool Attack()
    {
        state = GamePad.GetState(PlayerIndex.One);
        if (Input.GetKey(mapping.attack) || state.Triggers.Left > 0f || state.Triggers.Right > 0f)
        {
            return true;
        }
        return false;
    }

        public  Vector2 Move()
        {
            float x = 0f;
            float y = 0f;
            if (Input.GetKey(mapping.up))
            {
                y = 1f;
            }
            if (Input.GetKey(mapping.down))
            {
                y = -1f;
            }
            if (Input.GetKey(mapping.right))
            {
                x = 1f;
            }
            if (Input.GetKey(mapping.left))
            {
                x = -1f;
            }
            x += Input.GetAxis(mapping.MoveHorizontal);

            y += Input.GetAxis(mapping.MoveVertical);

            return new Vector2(x, y);

        }

        /// <summary>
        /// Retrun MousePos or Joy Left
        /// </summary>
        /// <returns></returns>
        public  Vector2 Rotation()
        {
            if (state.IsConnected)
            {
                Vector2 vec2;

                vec2.x = state.ThumbSticks.Right.X;

                vec2. y = state.ThumbSticks.Right.Y;

                //x = Input.GetAxis(mapping.RotationX);

                //y = Input.GetAxis(mapping.RotationY);

                return vec2;
            }
            else
            {
                return Input.mousePosition;
            }

         

        }

    // public static 
    }
}
[System.Serializable]
public class Mapping
{

    private Mapping() { }

    private static Mapping get;
    public static Mapping Get
    {
        get
        {
            if (get == null)
            {
                get = new Mapping();
            }
            return get;
        }
    }

    public KeyCode up = KeyCode.W;

    public KeyCode down = KeyCode.S;

    public KeyCode left = KeyCode.A;

    public KeyCode right = KeyCode.D;

    public KeyCode attack = KeyCode.Space;

    public string joyattack = "Attack";

    public string MoveHorizontal = "Horizontal";

    public string MoveVertical = "Vertical";

   // public string RotationX = "RotationX";

   // public string RotationY = "RotationY";
}


