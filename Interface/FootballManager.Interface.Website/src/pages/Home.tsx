import { Carousel, Image, Card, Container, Row, Col, Button, OverlayTrigger, Tooltip, Alert } from "react-bootstrap"
import { FaExpand, FaPlus, FaTrash } from "react-icons/fa";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { selectState, fetchAsync, onCloseError, NewsProps, onShowAdd, addAsync, onShowDetail, onSelectedId, deleteAsync } from '../slices/newsSlice'
import { useEffect } from "react";
import AddNews from "../components/AddNews";
import NewsDetailModal from "../components/NewsDetailModal";
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
                        <Row>
                            <Col className="d-flex justify-content-end">
                                <Button className="me-2" onClick={() => { removeNews(item.id) }} variant="danger"><FaTrash /></Button>
                                <Button onClick={() => {
                                    dispatch(onSelectedId(item.id))
                                    dispatch(onShowDetail(true))
                                }} variant="success"><FaExpand /></Button>
                            </Col>
                        </Row>
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

    const addNews = (model: NewsProps) => {
        dispatch(addAsync(model)).unwrap()
            .then(values => dispatch(onShowAdd(false)))
            .then(values => fetch())
            .then(values => {
            })
            .catch(error => {
            })
    }

    const removeNews = (id: number) => {
        dispatch(deleteAsync(id)).unwrap()
            .then(values => fetch())
            .then(values => { })
            .catch(error => { })
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
            {
                state.isShowDetail &&
                <NewsDetailModal model={state.data.find(i => i.id === state.selectedId)} show={state.isShowDetail} onClose={() => { dispatch(onShowDetail(false)) }} />
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