namespace FableRustRaytracerNet6iOS

open System
open System.Runtime.InteropServices
open CoreGraphics
open UIKit
open Foundation


module Native =
    let [<Literal>] DllName = "__Internal"
    
    [<DllImport(DllName, CallingConvention=CallingConvention.Cdecl)>]
    //(value1: i32, value2: i32) -> i32
    extern int32 add_values(nativeint value1, nativeint value2)
    
    [<DllImport(DllName, CallingConvention=CallingConvention.Cdecl)>]
    //(x: i32, y: i32, w: i32, h: i32, angle: f64) -> *const u8
    extern IntPtr render_scene(nativeint x, nativeint y, nativeint w, nativeint h, float angle)
    
    let getUIImageForRGBAData width height (dataPtr:IntPtr) (dataLen:int) =
        // https://gist.github.com/irskep/e560be65163efcb04115
        let bytesPerPixel = 4
        let scanWidth = bytesPerPixel * width
        let provider = new CGDataProvider(dataPtr, dataLen)
        let colorSpaceRef = CGColorSpace.CreateDeviceRGB()
        let bitMapInfo = CGBitmapFlags.Last
        let renderingIntent = CGColorRenderingIntent.Default
        let imageRef = new CGImage(width,height,8, bytesPerPixel * 8, scanWidth, colorSpaceRef, bitMapInfo, provider,null,false,renderingIntent)
        new UIImage(imageRef)

type MyViewController() =
    inherit UIViewController()
    
    
    let renderAnimatedImage (degrees:float) =
        async {
//            printfn $"Creating image for {degrees} degrees"
            let xa, ya, wa, ha = ( 0, 0, 128, 128)
            let lenA = wa * ha * 4
            let angleA = (float degrees) * Math.PI / 180.
            let ret_ptr_A = Native.render_scene(nativeint xa,nativeint ya,nativeint wa,nativeint ha,angleA)
            return Native.getUIImageForRGBAData wa ha ret_ptr_A lenA
        }
    override this.ViewDidLoad() =
        let x, y, w, h = ( 0, 0, 512, 512)
        let angle = 0.0
        let len = w * h * 4        
        let ret_ptr = Native.render_scene(nativeint x,nativeint y,nativeint w,nativeint h,angle)
        let ptr_hex = String.Format("{0:X8}", ret_ptr.ToInt64())
        printfn $"Native big render scene returned 0x{ptr_hex}"
        let imageView = new UIImageView()
        imageView.Frame <- CGRect(float x, float y, float w, float h)
        imageView.Image <- Native.getUIImageForRGBAData w h ret_ptr len
        this.View.AddSubview(imageView)
        let animationView = new UIImageView()
        animationView.Frame <- CGRect(0., 512., 128., 128.)
        let tasks = ResizeArray()
        printfn "Creating animation images in parallel..."
        for degrees = 720 downto 0 do
              tasks.Add(renderAnimatedImage (float degrees))
              
        let images = Async.Parallel(tasks) |> Async.RunSynchronously
        
        printfn $"Finished, created {images.Length} images"
        animationView.AnimationImages <- images
        this.View.AddSubview(animationView)
        animationView.StartAnimating()
    
[<Register(nameof AppDelegate)>]
type AppDelegate() =
    inherit UIApplicationDelegate()
       
    override val Window = null with get, set

    override this.FinishedLaunching(_, _) =
        printfn $" Native add_values: 5 + 6 = {Native.add_values(nativeint 5,nativeint 6)}"
        this.Window <- new UIWindow(UIScreen.MainScreen.Bounds)
        let vc = new MyViewController()
        this.Window.RootViewController <- vc
        this.Window.MakeKeyAndVisible()
        true
        
       
