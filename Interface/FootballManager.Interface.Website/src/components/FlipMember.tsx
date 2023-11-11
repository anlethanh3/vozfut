import { Button, Modal, Row, Col, Card } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { MemberProps } from '../slices/memberSlice';
import ReactCardFlip from 'react-card-flip';
import shuffle from 'lodash/shuffle'

export default function FlipMember(props: { show: boolean, initial: MemberProps[], onSubmit: () => void, onClose: () => void }) {
    const { show, onSubmit, onClose, initial, } = props
    const [state, setState] = useState<boolean[]>([])
    const [members, setMembers] = useState<MemberProps[]>([])
    const onValid = () => {
        onSubmit()
    }

    const onFlipItem = (i: number) => {
        state[i] = true
        setState([...state])
    }

    useEffect(() => {
        var item: boolean[] = []
        var list = shuffle(initial)
        for (let i = 0; i < list.length; i++) {
            item.push(false)
        }
        setMembers([...list])
        setState([...item])
    }, [initial])

    function drawGrid(): JSX.Element[] {
        var colItems: JSX.Element[] = [];
        var rowItems: JSX.Element[] = [];
        var col = 4
        var row = members.length / col
        for (let i = 0; i < row - 1; i++) {
            for (let j = 0; j < col; j++) {
                let num = (i * col) + j
                let member = members[num]
                let item = (
                    <Col className='pb-2'>
                        <ReactCardFlip isFlipped={state[num]} flipDirection="vertical">
                            <Card style={{ height: '5em', cursor: 'pointer' }} onClick={() => { onFlipItem(num) }}>
                                <Card.Body>
                                    <Card.Title>{num + 1}</Card.Title>
                                </Card.Body>
                            </Card>

                            <Card style={{ height: '5em' }} >
                                <Card.Body>
                                    <Card.Title>{member.name}</Card.Title>
                                    <Card.Subtitle>Elo: +{member.elo}</Card.Subtitle>
                                </Card.Body>
                            </Card>
                        </ReactCardFlip>
                    </Col>
                )
                colItems.push(item)
            }
            rowItems.push((<Row>{colItems}</Row>))
            colItems = []
        }
        return rowItems
    }

    return (<>
        <Modal show={show} fullscreen onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Flip Member</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    drawGrid()
                }
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}