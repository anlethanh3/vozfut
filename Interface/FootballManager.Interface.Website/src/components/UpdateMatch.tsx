import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { MatchProps } from '../slices/matchSlice';

export default function UpdateMatch(props: { show: boolean, initData: MatchProps | undefined, onSubmit: (model: MatchProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, initData } = props
    const [match, setMatch] = useState<MatchProps>({ name: '', description: '', teamCount: 3, teamSize: 5, id: 0 })
    const [valid, setValid] = useState({ name: true });
    const onValid = () => {
        if (match.name === '') {
            setValid({ ...valid, name: false })
            return
        }
        onSubmit(match)
    }
    useEffect(() => {
        if (initData) {
            setMatch({ ...initData })
        }
    }, [initData])
    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update a match</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="editMatch.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={!valid.name} placeholder="Input name" value={match.name} onChange={(e) => setMatch({ ...match, name: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Name is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editMatch.Description">
                        <FloatingLabel controlId="floatingDescription" label="Description">
                            <Form.Control type="text" placeholder="Input description" value={match.description} onChange={(e) => setMatch({ ...match, description: e.target.value })} />
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editMatch.TeamCount">
                        <FloatingLabel controlId="floatingTeamCount" label="TeamCount">
                            <Form.Select value={match.teamCount} onChange={(e) => setMatch({ ...match, teamCount: parseInt(e.target.value) })}>
                                {
                                    [3, 4].map(value =>
                                        <option key={`teamcount.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editMatch.TeamSize">
                        <FloatingLabel controlId="floatingTeamSize" label="TeamSize">
                            <Form.Select value={match.teamSize} onChange={(e) => setMatch({ ...match, teamSize: parseInt(e.target.value) })}>
                                {
                                    [5, 7, 11].map(value =>
                                        <option key={`teamcount.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="editMatch.HasTeamRival">
                            <Form.Switch label="HasTeamRival" checked={match.hasTeamRival} onChange={(e) => setMatch({ ...match, hasTeamRival: e.target.checked })} />
                    </Form.Group>
                </Form>
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