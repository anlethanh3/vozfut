import { Button, Modal, Form, FloatingLabel, } from 'react-bootstrap';
import { useState, useEffect } from 'react';
import { NewsProps } from '../slices/newsSlice';

export default function UpdateNews(props: { show: boolean, model: NewsProps | undefined, onSubmit: (model: NewsProps) => void, onClose: () => void }) {
    const { show, model, onSubmit, onClose } = props
    const [news, setNews] = useState<NewsProps>({ id: 0, title: '', content: '', imageUris: '' })
    const [valid, setValid] = useState({ title: true });
    const onValid = () => {
        if (news.title === '') {
            setValid({ ...valid, title: false })
            return
        }
        onSubmit(news)
    }
    useEffect(() => {
        if (model) {
            setNews(model)
        }
    }, [model])

    return (<>
        <Modal size='lg' show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update a News</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addNews.Title">
                        <FloatingLabel controlId="floatingTitle" label="Title">
                            <Form.Control type="text" required isInvalid={!valid.title} placeholder="Input name" value={news.title} onChange={(e) => setNews({ ...news, title: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Title is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addNews.content">
                        <Form.Control as="textarea" rows={5} placeholder="Input content" value={news.content} onChange={(e) => setNews({ ...news, content: e.target.value })} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addNews.imageUris">
                        <Form.Control as="textarea" placeholder='Input Image Ids per new line' rows={4} value={news.imageUris} onChange={(e) => setNews({ ...news, imageUris: e.target.value })} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant='secondary' onClick={onClose}>Close</Button >
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}