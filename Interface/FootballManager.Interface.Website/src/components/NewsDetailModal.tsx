import { Button, Modal, Image, Carousel, } from 'react-bootstrap';
import { useState, useEffect } from 'react';
import { NewsProps } from '../slices/newsSlice';

export default function NewsDetailModal(props: { show: boolean, model: NewsProps | undefined, onClose: () => void }) {
    const { show, model, onClose } = props
    const [news, setNews] = useState<NewsProps>({ id: 0, title: '', content: '', imageUris: '' })
    
    useEffect(() => {
        if (model) {
            setNews(model)
        }
    }, [model])

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

    return (<>
        <Modal fullscreen show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>{news.title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <pre>{news.content}</pre>
                <Slider data={news}/>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='secondary' onClick={onClose}>Close</Button >
            </Modal.Footer>
        </Modal>
    </>)
}