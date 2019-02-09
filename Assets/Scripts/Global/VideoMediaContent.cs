using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoMediaContent : MonoBehaviour, IPointerClickHandler
{

    private VideoPlayer videoPlayer;

    /* METODO PARA CONFIGURAR UN VIDEOPLAYER PARA REPRODUCIR UN VIDEO DADO UNA URL */
    public void ConfigureVideoPlayer(string url)
    {
        var rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.url = url;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = rt;
        this.GetComponent<RawImage>().texture = rt;
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;
        videoPlayer.prepareCompleted += VideoPlayerPrepared;
        videoPlayer.loopPointReached += VideoPlayerEnded;
    }

    /* ##################  METODO PARA PAUSAR EL VIDEO EN EL PRIMER FRAME NO OSCURO DEL VIDEO UNA VEZ ESTA LISTO  ################## */
    public void VideoPlayerPrepared(VideoPlayer videoPlayer)
    {
        videoPlayer.frame = 2;
        videoPlayer.Pause();
    }

    /* ##################  METODO PARA REINICIAR EL VIDEO CUANDO SE TERMINA  ################## */
    public void VideoPlayerEnded(VideoPlayer videoPlayer)
    {
        videoPlayer.frame = 1;
        videoPlayer.Pause();
    }

    /* ##################  METODO PARA EJECUTAR O PAUSAR EL VIDEO CADA VEZ QUE SE TOCA  ################## */
    public void OnPointerClick(PointerEventData eventData)
    {
        if (videoPlayer.isPrepared){
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
            else
                videoPlayer.Play();
        }
        return;
    }
}
