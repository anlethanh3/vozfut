import { Carousel, Image } from "react-bootstrap"

export interface Photo {
    uri: string,
    title: string,
    description: string,
}

export default function Home() {
    const images: Photo[] = [

        {
            uri: '1-11G8FaCF11AQY8e7t4l2qvMYmIfNY9R',
            title: '27/08/2023 - Sân vận động Gia Định',
            description: '2A Phan Chu Trinh, Phường 12, Bình Thạnh, Thành phố Hồ Chí Minh'
        },
        {
            uri: '1XaXAF748LhyETpvTIt5sN3NCaSqS0nkb',
            title: '6/8/2023 - Sân bóng đá 367',
            description: '168A Hoàng Hoa Thám, Phường 12, Tân Bình, Thành phố Hồ Chí Minh, Việt Nam'
        },
        {
            uri: '1sPvLQSiQxjolJICC2SQXBU7zsmI1fZdl',
            title: '6/8/2023 - Sân bóng đá 367',
            description: '168A Hoàng Hoa Thám, Phường 12, Tân Bình, Thành phố Hồ Chí Minh, Việt Nam'
        },
        {
            uri: '12EyfF8H0S2s6OEb7NQmuI3Oph3Um0D5r',
            title: '6/8/2023 - Sân bóng đá 367',
            description: '168A Hoàng Hoa Thám, Phường 12, Tân Bình, Thành phố Hồ Chí Minh, Việt Nam'
        },
        {
            uri: '1ykvBN_i7xaRVuwoe1BPAGEue1dS8vOSS',
            title: '6/8/2023 - Sân bóng đá 367',
            description: '168A Hoàng Hoa Thám, Phường 12, Tân Bình, Thành phố Hồ Chí Minh, Việt Nam'
        },
    ]
    return (
        <div>
            <h1>Events</h1>
            <Carousel>
                {
                    images.map((item, index) =>
                        <Carousel.Item key={`image-${index}`} >
                            <Image src={`https://drive.google.com/uc?export=view&id=${item.uri}`} fluid />
                            <Carousel.Caption>
                                <h3>{item.title}</h3>
                                <p>{item.description}</p>
                            </Carousel.Caption>
                        </Carousel.Item>
                    )
                }
            </Carousel>
        </div>
    )
}