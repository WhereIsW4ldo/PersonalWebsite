use std::{thread, time};

fn main() {
    let test_number: i16 = 100;

    println!("Hello, world!");

    println!("{test_number}");

    let one_second = time::Duration::from_secs(1);

    loop {
        println!("another run");
        thread::sleep(one_second);
    }
}
