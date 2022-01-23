use crate::RayTracer::RayTracerDemo;

use std::rc::Rc;
use fable_library_rust::*;

const WMAX: i32 = 2048;
const HMAX: i32 = 2048;
const BUF_LEN: i32 = WMAX * HMAX * 4;

fn get_buffer() -> Rc<MutCell<Vec<u8>>> {
    thread_local! {
        static DATA: Rc<MutCell<Vec<u8>>> = Native::arrayCreate(&BUF_LEN, &0u8);
    }
    DATA.with(|data| data.clone())
}

#[no_mangle]
pub unsafe extern "C" fn render_scene(x: i32, y: i32, w: i32, h: i32, angle: f64) -> *const u8 {
    let buffer = get_buffer();
    RayTracerDemo::renderScene(&buffer, &x, &y, &w, &h, &angle);
    // println!("buf len {}",buffer.as_slice().len());
    buffer.as_ptr()
}

#[no_mangle]
pub unsafe extern "C" fn add_values(
    value1: i32,
    value2: i32,
) -> i32 {
    println!("Passed value1: {}, value2: {}", value1, value2);
    value1 + value2
}