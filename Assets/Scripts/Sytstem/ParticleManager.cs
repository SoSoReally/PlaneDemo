using System.Collections.Generic;
using UnityEngine;
namespace ParticleManager
{
    public class GameParticleSystem
    {
        bool once = false;
        private readonly Dictionary<EffectEnum, EffectBase> effects = new Dictionary<EffectEnum, EffectBase>();

        private GameParticleSystem()
        {

        }

        private static GameParticleSystem pmc;
        public  static GameParticleSystem Get { get {
                if (pmc ==null)
                {
                    pmc = new GameParticleSystem();
                }
                return pmc;
            } }

        public void LoadAllParticle()
        {
            if (once)
            {
                return;
            }
            var list = Resources.LoadAll<EffectBase>(GamePath.AllParticle);
            foreach (var item in list)
            {
                effects.Add( item.EffectEnum,item);
                GameManager.Get.Spawn_system.RegisterSpawn(item.Spawn_ID);
            }

            once = true;
        }

        public EffectBase GetParticleAsset(EffectEnum effect)
        {

            return effects[effect];
        }

        public EffectBase PlayOnPos(EffectEnum effect, Vector3 pos, Vector3? Rotation=null, bool withChildren=false)
        {
            var ptr = GetParticleAsset(effect);
            var particle = GameManager.Get.Spawn_system.OnSpawn(ptr.Spawn_ID).GetComponent<EffectBase>();
            particle.transform.position = pos;
            if (Rotation!=null)
            {
                particle.transform.localEulerAngles = (Vector3)Rotation;
            }
            particle.gameObject.SetActive(true);
            if (withChildren)
            {
                particle.Play(withChildren);
            }
            particle.Play();
            return particle;
        }
        

    }
}


