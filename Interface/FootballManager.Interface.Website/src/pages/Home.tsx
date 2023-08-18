import { Carousel, Image } from "react-bootstrap"
export default function Home() {
    const images = [
        '1XaXAF748LhyETpvTIt5sN3NCaSqS0nkb',
        '1sPvLQSiQxjolJICC2SQXBU7zsmI1fZdl',
        '12EyfF8H0S2s6OEb7NQmuI3Oph3Um0D5r',
        '1ykvBN_i7xaRVuwoe1BPAGEue1dS8vOSS',
    ]
    return (
        <div>
            <h1>Events</h1>
            <Carousel>
                {
                    images.map((item, index) =>
                        <Carousel.Item key={`image-${index}`} >
                            <Image src={`https://drive.google.com/uc?export=view&id=${item}`} fluid />
                            <Carousel.Caption>
                                <h3>6/8/2023 - Sân bóng đá 367</h3>
                                <p>168A Hoàng Hoa Thám, Phường 12, Tân Bình, Thành phố Hồ Chí Minh, Việt Nam</p>
                            </Carousel.Caption>
                        </Carousel.Item>
                    )
                }
            </Carousel>
        </div>
    )
}