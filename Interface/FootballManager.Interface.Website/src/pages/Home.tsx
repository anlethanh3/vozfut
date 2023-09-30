import { Carousel, Image, Card, Container, Row, Col, Button, OverlayTrigger, Tooltip, Alert } from "react-bootstrap"
import { FaPlus } from "react-icons/fa";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { selectState, fetchAsync, onCloseError, NewsProps, onShowAdd, addAsync, deleteAsync, updateAsync } from '../slices/newsSlice'
import { useEffect } from "react";
import AddNews from "../components/AddNews";
export interface Photo {
    uri: string,
    title: string,
    description: string,
}

export default function Home() {
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ name: state.search.name, pageIndex: state.pageIndex, pageSize: state.pageSize }))
            .unwrap()
            .then(values => {

            })
            .catch(error => {

            })
    }

    useEffect(() => {
        console.log('effect', state)
        fetch()
        return function cleanup() {
            console.log('unmount', state)
        }

    }, [state.pageIndex, state.pageSize])

    function News(props: { data: NewsProps[] }) {
        let { data } = props
        let items: JSX.Element[] = []
        var cols: JSX.Element[] = []
        data.forEach(item => {
            let element =
                (<Card>
                    <Slider data={item} />
                    <Card.Body>
                        <Card.Title>{item.title}</Card.Title>
                        <Card.Text> {item.content}</Card.Text>
                    </Card.Body>
                </Card>)
            cols.push((<Col>{element}</Col>))
            if (cols.length === 3) {
                items.push((<Row className="mb-2">{cols}</Row>))
                cols = []
            }
        })
        if (cols.length >= 0) {
            items.push((<Row className="mb-2">{cols}</Row>))
            cols = []
        }
        return (<Container>{items}</Container>)
    }

    function Slider(props: { data: NewsProps }) {
        let { data } = props
        let uris = data.imageUris.split(';')
        return (<Carousel>
            {
                uris.map((item, index) => {
                    return <Carousel.Item key={`image-${index}`} >
                        <Image src={`https://drive.google.com/uc?export=view&id=${item}`} fluid />
                    </Carousel.Item>
                }
                )
            }
        </Carousel>)
    }
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

    const addNews = (model: NewsProps) => {
        dispatch(addAsync(model)).unwrap()
            .then(values => fetch())
            .then(values => {
            })
            .catch(error => {
            })
    }
    return (
        <>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch(onCloseError())} dismissible>
                    {state.error}
                </Alert>
            }
            {
                state.isShowAdd &&
                <AddNews onSubmit={(news) => { addNews(news) }} show={state.isShowAdd} onClose={() => { dispatch(onShowAdd(false)) }} />
            }
            <Row>
                <Col><h1>News</h1></Col>
                <Col className="d-flex align-items-center justify-content-end">
                    <OverlayTrigger overlay={<Tooltip>Add news</Tooltip>}>
                        <Button variant="primary" onClick={() => { dispatch(onShowAdd(true)) }}><FaPlus /></Button>
                    </OverlayTrigger>
                </Col>
            </Row>
            <News data={state.data} />
        </>
    )
}