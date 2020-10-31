namespace Engine
{
    /// <summary>
    /// This is the base for any game object
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// The unique Id of this object
        /// </summary>
        public int Id { get; protected set; }
        
        #region LIFECYCLE

        /// <summary>
        /// Called when this object is created for the first time
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Called when this object is destroyed
        /// </summary>
        public abstract void Destroy();

        #endregion
    }
}