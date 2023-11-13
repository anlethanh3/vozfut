import { Button, Modal, Row, Col, Card, Accordion } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { MemberProps } from '../slices/memberSlice';
import ReactCardFlip from 'react-card-flip';
import shuffle from 'lodash/shuffle'
import AddMemberClass, { AddClassMemberProps } from './AddMemberClass';
import { MatchProps } from '../slices/matchSlice';

interface FlipMemberProps {
    isFlip: boolean,
    member: MemberProps,
    teamId: number,
}

interface FlipMemberState {
    isShowAdd: boolean,
    data: FlipMemberProps[][],
    teamId: number,
}

export default function FlipMember(props: { show: boolean, match: MatchProps, initial: MemberProps[], onSubmit: () => void, onClose: () => void }) {
    const { show, onSubmit, onClose, initial, match } = props
    const [state, setState] = useState<FlipMemberState>({ data: [[], [], []], isShowAdd: false, teamId: 0 })
    const onValid = () => {
        onSubmit()
    }

    const onFlipItem = (data: FlipMemberProps) => {
        data.isFlip = true
        data.teamId = state.teamId
        state.teamId += 1
        console.log(state.teamId, match.teamCount)
        if (state.teamId >= match.teamCount) {
            state.teamId = 0
        }
        setState({ ...state })
    }

    useEffect(() => {
        var list: FlipMemberProps[][] = [[], [], []]
        initial.forEach((i) => {
            list[2].push({ isFlip: false, member: i, teamId: -1 })
        })
        let slist = shuffle(list[2])
        list[2] = slist
        setState({ ...state, data: list, isShowAdd: false, })
    }, [initial])

    function drawGrid(members: FlipMemberProps[]): JSX.Element[] {
        var colItems: JSX.Element[] = [];
        var rowItems: JSX.Element[] = [];
        var col = 4
        var row = members.length / col
        for (let i = 0; i < row; i++) {
            for (let j = 0; j < col; j++) {
                let num = (i * col) + j
                if (num >= members.length) { continue; }
                let data = members[num]
                let member = members[num].member
                let isFlip = members[num].isFlip
                let item = (
                    <Col className='pb-2'>
                        <ReactCardFlip isFlipped={isFlip} flipDirection="vertical">
                            <Card style={{ height: '8em', cursor: 'pointer' }} onClick={() => { onFlipItem(data) }}>
                                <Card.Body>
                                    <Card.Title>{num + 1}</Card.Title>
                                </Card.Body>
                            </Card>
                            <Card style={{ height: '8em' }} >
                                <Card.Body>
                                    <Card.Title>{member.name}</Card.Title>
                                    <Card.Subtitle>Elo: +{member.elo}</Card.Subtitle>
                                </Card.Body>
                                <Card.Footer>Team: {data.teamId + 1}</Card.Footer>
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

    function drawTabs(): JSX.Element {
        return (
            <Accordion defaultActiveKey="2">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Class SSS</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[0])
                        }
                    </Accordion.Body>
                </Accordion.Item>
                <Accordion.Item eventKey="1">
                    <Accordion.Header>Class S</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[1])
                        }
                    </Accordion.Body>
                </Accordion.Item>
                <Accordion.Item eventKey="2">
                    <Accordion.Header>Class A</Accordion.Header>
                    <Accordion.Body>
                        {
                            drawGrid(state.data[2])
                        }
                    </Accordion.Body>
                </Accordion.Item>
            </Accordion>
        )
    }

    function setShowAdd(isShow: boolean) {
        setState({ ...state, isShowAdd: isShow })
    }

    function onAddMemberClass(data: AddClassMemberProps) {
        let array = initial.filter(i => i.id === data.memberId);
        if (array.length <= 0) {
            return
        }
        let member = array[0]
        let list: FlipMemberProps[][] = []
        state.data.forEach(i => {
            list.push(i.filter(k => k.member.id !== member.id))
        })
        list[data.classId].push({ isFlip: false, member: member, teamId: -1 })
        list[data.classId] = shuffle(list[data.classId])
        setState({ ...state, isShowAdd: false, data: list })
    }

    return (<>
        {
            state.isShowAdd &&
            <AddMemberClass members={initial} onSubmit={(data) => { onAddMemberClass(data) }} onClose={() => { setShowAdd(false) }} show={state.isShowAdd} />
        }
        <Modal show={show} fullscreen onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Flip Member: Turn Team {state.teamId+1} pick</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    drawTabs()
                }
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button onClick={() => { setShowAdd(true) }}>Add Member to Class</Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}